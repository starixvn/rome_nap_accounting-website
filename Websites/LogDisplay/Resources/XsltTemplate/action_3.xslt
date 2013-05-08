<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>
    <xsl:template match="Acc_CustomerUpdUserInfo">
        <table width="100%" class="tblDesc">
            <tr>
                <td class="colW1">{lbl:firstname}</td>
                <td class="colW2">
                    <xsl:value-of select="@fname" />
                </td>
                <td class="colW1">{lbl:lname}</td>
                <td class="colW2">
                    <xsl:value-of select="@lname" />
                </td>
                <td class="colW1">{lbl:address}</td>
                <td class="colW2">
                    <xsl:value-of select="@address" />
                </td>
            </tr>
            <tr>
                <td class="colW1">{lbl:Postal}</td>
                <td class="colW2">
                    <xsl:value-of select="@postal" />
                </td>
                <td class="colW1">{lbl:city}</td>
                <td class="colW2">
                    <xsl:value-of select="@city" />
                </td>
                <td class="colW1">{lbl:state}</td>
                <td class="colW2">
                    <xsl:value-of select="@state" />
                </td>
                <!--<td>
                    <b>

                        -Postal: <xsl:value-of select="@postal" />
                        <br/>
                        -City: <xsl:value-of select="@city" />
                        <br/>
                        -State: <xsl:value-of select="@state" />
                        <br/>
                        -Country: <xsl:value-of select="@country" />
                        <br/>
                        -Phone: <xsl:value-of select="@phone" />
                    </b>
                </td>
                <td>
                </td>-->
            </tr>
            <tr>
                <td class="colW1">{lbl:country}</td>
                <td class="colW2">
                    <xsl:value-of select="@country" />
                </td>
                <td class="colW1">{lbl:mobilephone}</td>
                <td class="colW2">
                    <xsl:value-of select="@phone" />
                </td>
                <td class="colTitle"></td>
                <td class="colValue">

                </td>
            </tr>
        </table>
    </xsl:template>
</xsl:stylesheet>