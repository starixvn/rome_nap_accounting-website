<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="acc_customerupdmasterfi_positiontaking">
        <table class="tblDesc">
            <xsl:for-each select="info">
                <tr class="headerDetail" onclick="$('#info{{tbodylogid}}').slideToggle();">
                    <td colspan="6">{lbl:custInfo}</td>
                </tr>
                <tr>
                    <td colspan="6">
                      <div style="display:block;" id="info{{tbodylogid}}">
                        <table class="tblDesc">
                          <tr >
                            <td class="colW1">{lbl:Updallmembers}</td>
                            <td class="colW2">
                              <xsl:value-of select="@updallmembers" />
                            </td>
                            <td class="colW1">{lbl:newspt}</td>
                            <td class="colW2">
                              <xsl:value-of select="@newspt" />
                            </td>
                            <td class="colW1">{lbl:SubAccName}</td>
                            <td class="colW2">
                              <xsl:value-of select="@subaccname" />
                            </td>
                          </tr>
                          <tr >
                            <td class="colW1">{lbl:Language}</td>
                            <td class="colW2">
                              <xsl:value-of select="@language" />
                            </td>
                            <td class="colW1">{lbl:IP}</td>
                            <td class="colW2">
                              <xsl:value-of select="@ip" />
                            </td>

                            <td class="colW1">{lbl:ErrCode}</td>
                            <td class="colW2">
                              <xsl:value-of select="@errcode" />
                            </td>
                          </tr>
                          <tr >
                            <td class="colW1">{lbl:ErrMsg}</td>
                            <td class="colW2">
                              <xsl:value-of select="@errmsg" />
                            </td>
                            <td class="colTitle"></td>
                            <td class="colValue">

                            </td>
                            <td class="colTitle"></td>
                            <td class="colValue">

                            </td>
                          </tr>
                        </table>
                      </div>
                    </td>
                </tr>
            </xsl:for-each>
            <xsl:for-each select="s">
                <tr class="headerDetail" onclick="$('#s{{tbodylogid}}').slideToggle();">
                    <td colspan="6">{lbl:s}</td>
                </tr>
                <tr>
                    <td colspan="6">
                      <div style="display:none;" id="s{{tbodylogid}}">
                        <table class="tblDesc">
                          <tr>
                            <td class="colW1">{lbl:v201spt_2}</td>
                            <td class="colW2">
                              <xsl:value-of select="@v201spt_2" />
                            </td>
                            <td class="colW1">{lbl:v201spt_3}</td>
                            <td class="colW2">
                              <xsl:value-of select="@v201spt_3" />
                            </td>
                            <td class="colTitle"></td>
                            <td class="colValue">

                            </td>
                          </tr>
                        </table>
                      </div>
                    </td>
                </tr>
            </xsl:for-each>
            <xsl:for-each select="au">
                <tr class="headerDetail" onclick="$('#au{{tbodylogid}}').slideToggle();">
                    <td colspan="6">{lbl:au}</td>
                </tr>
                <tr>
                    <td colspan="6">
                      <div style="display:none;" id="au{{tbodylogid}}">
                        <table class="tblDesc">
                          <tr >
                            <td class="colW1">{lbl:a_v201spt_2}</td>
                            <td class="colW2">
                              <xsl:value-of select="@a_v201spt_2" />
                            </td>
                            <td class="colW1">{lbl:a_v201spt_3}</td>
                            <td class="colW2">
                              <xsl:value-of select="@a_v201spt_3" />
                            </td>
                            <td class="colTitle"></td>
                            <td class="colValue">

                            </td>
                          </tr>
                        </table>
                      </div>
                    </td>
                </tr>
            </xsl:for-each>
            <xsl:for-each select="ia">
                <tr class="headerDetail" onclick="$('#ia{{tbodylogid}}').slideToggle();">
                    <td colspan="6">{lbl:isaPTonM}</td>
                </tr>
                <tr>
                    <td colspan="6">
                      <div style="display:none;" id="ia{{tbodylogid}}">
                        <table class="tblDesc">
                          <tr>
                            <td class="colW1">{lbl:isa_v201spt_2}</td>
                            <td class="colW2">
                              <xsl:value-of select="@isa_v201spt_2" />
                            </td>
                            <td class="colW1">{lbl:isa_v201spt_3}</td>
                            <td class="colW2">
                              <xsl:value-of select="@isa_v201spt_3" />
                            </td>
                            <td class="colTitle"></td>
                            <td class="colValue">

                            </td>
                          </tr>
                        </table>
                      </div>
                    </td>
                </tr>
            </xsl:for-each>
            <xsl:for-each select="m">
                <tr class="headerDetail" onclick="$('#m{{tbodylogid}}').slideToggle();">
                    <td colspan="6">{lbl:m}</td>
                </tr>
                <tr>
                    <td colspan="6">
                      <div style="display:none;" id="m{{tbodylogid}}">
                        <table class="tblDesc">
                          <tr >
                            <td class="colW1">{lbl:v201mpt_2}</td>
                            <td class="colW2">
                              <xsl:value-of select="@v201mpt_2" />
                            </td>
                            <td class="colW1">{lbl:v201mpt_3}</td>
                            <td class="colW2">
                              <xsl:value-of select="@v201mpt_3" />
                            </td>
                            <td class="colTitle"></td>
                            <td class="colValue">

                            </td>
                          </tr>
                        </table>
                      </div>
                    </td>
                </tr>
            </xsl:for-each>
        </table>
    </xsl:template>
</xsl:stylesheet>
